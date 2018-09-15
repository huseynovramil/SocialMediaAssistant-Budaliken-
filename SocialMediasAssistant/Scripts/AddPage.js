let pages = [];
$(document).ready(function () {
    window.fbAsyncInit = function () {
        FB.init({
            appId: '894196017436373',
            cookie: true,
            xfbml: true,
            version: 'v3.1'
        });

        FB.AppEvents.logPageView();
        let accessToken;
        FB.getLoginStatus(function (response) {
            if (response.status === 'connected') {
                accessToken = response.authResponse.accessToken;

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
                        let selectPostButton = $('#selectPost');
                        let page_id = $('#page_id');
                        let page_access_token = $('#page_access_token');
                        $('.dropdown-item').click(function () {
                            let index = $(this).attr('data-value');
                            selected.val(index);
                            selected.text($(this).text());
                            page_id.val(pages[index].id);
                            page_access_token.val(pages[index].access_token);
                        });
                    }
                );
            }
        });
    };

    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) { return; }
        js = d.createElement(s); js.id = id;
        js.src = "https://connect.facebook.net/en_US/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));

});