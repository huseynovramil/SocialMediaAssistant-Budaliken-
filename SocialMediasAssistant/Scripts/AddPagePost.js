let pages = [];
$(document).ready(function () {
    window.fbAsyncInit = function () {
        FB.init({
            appId: '253756692014883',
            cookie: true,
            xfbml: true,
            version: 'v3.1'
        });

        FB.AppEvents.logPageView();
        let accessToken;
        FB.getLoginStatus(function (response) {
            if (response.status === 'connected') {
                accessToken = response.authResponse.accessToken;
                FB.api(
                    '/me',
                    'GET',
                    { 'access_token': accessToken },
                    function (response) {
                        $.ajax({
                            url: "SaveAccessTokenAsync",
                            method: "POST",
                            data: {
                                accessToken: accessToken,
                                name: response.name
                            },
                            success: function (response) {
                                console.log(response);
                                let dropdownList = $('#pages .dropdown-menu');
                                FB.api(
                                    '/me/accounts',
                                    'GET',
                                    { 'access_token': accessToken },
                                    function (innerResponse) {
                                        $.each(innerResponse.data, function (index, value) {
                                            pages.push(value);
                                            let listItem = `<a class="dropdown-item" href="#" data-value="` + index + `">` + value.name + `</a>`;
                                            dropdownList.append(listItem);
                                        });
                                        let selected = $('#pagesSelected');
                                        let selectPostButton = $('#selectPost,#postLink');
                                        let page_id = $('#page_id');
                                        let page_access_token = $('#page_access_token');
                                        $('.dropdown-item').click(function () {
                                            let index = $(this).attr('data-value');
                                            selected.val(index);
                                            selected.text($(this).text());
                                            page_id.val(pages[index].id);
                                            page_access_token.val(pages[index].access_token);
                                            selectPostButton.removeAttr('disabled');
                                            FB.api(
                                                '/' + pages[index].id + '/posts',
                                                'GET',
                                                {
                                                    'access_token': pages[index].access_token
                                                },
                                                function (response) {
                                                    console.log(response);
                                                    let posts = $('#posts div .row');
                                                    posts.empty();
                                                    $.each(response.data, function (indexPost, value) {
                                                        let link = "https://www.facebook.com/";
                                                        let idOfPage = pages[index].id;
                                                        let idOfPost = value.id.substr(value.id.lastIndexOf('_') + 1);
                                                        link = link + idOfPage + '/posts/' + idOfPost;
                                                        setContent(link, posts);
                                                    });
                                                    FB.XFBML.parse();
                                                }
                                            );
                                        });
                                    });
                            }
                        });
                    }

                );
            }
        });

        function setContent(link, posts) {
            let post = `<label class="btn btn-light col-md-12">
                    <div class="row">
                   <input type="radio" class="col-md-12" name="postLinks" autocomplete="off" value="`+ link + `">
            <div class="col-md-12 justify-content-center d-flex" style="padding-top:2%">
            <div class="fb-post" data-href="`+ link + `"
            data-width="350" data-show-text="true">
            <blockquote cite="`+ link + `"
                class="fb-xfbml-parse-ignore"></blockquote>
            </div>
            </div>
            </div>
               </label>
                <hr/>
`;
            posts.append(post);
        }
    };
    let postLink = $('#postLink');
    $('#postsModal').on('hidden.bs.modal', function (e) {
        let link = $('input[name=postLinks]:checked').val();
        postLink.val(link);
        postLink.text(link);
    });
    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) { return; }
        js = d.createElement(s); js.id = id;
        js.src = "https://connect.facebook.net/en_US/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));

});