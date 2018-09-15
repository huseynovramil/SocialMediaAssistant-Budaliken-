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
            FB.api(
                '/me',
                'GET',
                { 'access_token': accessToken },
                function (response) {
                    $.ajax({
                        url: "/Facebook/SaveAccessTokenAsync",
                        method: "POST",
                        data: {
                            accessToken: accessToken,
                            name: response.name
                        },
                        function(response) {
                            console.log(response);
                        }
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

$(document).ready(function () {
    $.ajax({
        url: ""
    });
});
