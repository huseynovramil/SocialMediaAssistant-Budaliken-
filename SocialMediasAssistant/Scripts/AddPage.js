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
                        let page_id = $('#page_id');
                        let page_link = $('#page_link');
                        let page_access_token = $('#page_access_token');
                        let page_points = $('#page_points');
                        let pageDropdownItem = $('#pages .dropdown-item');
                        pageDropdownItem.click(function () {
                            let index = $(this).attr('data-value');
                            let selected = $(this).parent('.dropdown-menu').siblings('.btn');
                            selected.val(index);
                            selected.text($(this).text());
                            page_id.val(pages[index].id);
                            page_link.val("https://facebook.com/" + pages[index].id);
                            page_access_token.val(pages[index].access_token);
                        });

                        $('#points .dropdown-item').click(function () {
                            let index = $(this).attr('data-value');
                            let selected = $(this).parent('.dropdown-menu').siblings('.btn');
                            selected.val(index);
                            selected.text($(this).text());
                            page_points.val($(this).text());
                        });

                        let pageId = page_id.val();
                        if (pageId) {
                            let pageIndex = pages.findIndex((value) => value.id === pageId);
                            let itemToSelect = $(pageDropdownItem.get(pageIndex));
                            itemToSelect.trigger('click');
                        }
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