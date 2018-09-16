/// <reference path="jquery-3.3.1.js" />
let lateContentLikes = 0;
let currentHasLiked = false;
let points = 0;
let currentContentIndex = 0;
$(document).ready(function () {
    window.fbAsyncInit = function () {
        FB.init({
            appId: '253756692014883',
            cookie: true,
            xfbml: true,
            version: 'v3.1'
        });

        FB.AppEvents.logPageView();

        getContent(true);

    };

    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) { return; }
        js = d.createElement(s); js.id = id;
        js.src = "https://connect.facebook.net/en_US/sdk.js";
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));



    let contents = [];
    async function getContent(isFirst) {
        return $.ajax({
            url: 'GetPagePosts',
            method: 'get',
            data: {
                "order": contents.length / 10
            },
            success: function (response) {
                $.each(response, function () {
                    contents.push(this);
                });
                if (isFirst) {
                    getHasLiked(currentContentIndex, SkipIfLiked);
                }
            }
        });
    }
    let cards = $('#cards');
    let pointsLabel = $('#points');
    function setContent() {
        pointsLabel.text(points);
        let card = `<div class="fb-post mw-100" data-href="`+ contents[currentContentIndex].Link + `"
            data-width="350" data-show-text="true">
            <blockquote cite="`+ contents[currentContentIndex].Link + `"
                class="fb-xfbml-parse-ignore"></blockquote>
        </div>`;
        cards.html(card);
        FB.XFBML.parse();
    }
    $('#refresh').click(function () {
        getHasLiked(currentContentIndex, IncreasePoints, true);
    });
    function IncreasePoints(response) {
        if (response) {
            points = points + 10;
        }
        currentContentIndex = currentContentIndex + 1;
        getHasLiked(currentContentIndex, SkipIfLiked);
    }

    function SkipIfLiked(response) {
        if (response) {
            currentContentIndex = currentContentIndex + 1;
            getHasLiked(currentContentIndex, SkipIfLiked);
        }
        else {
            setContent();
        }
    }
    async function getHasLiked(index, callback, increasePoints=false) {
        $.ajax({
            url: "HasLikedPagePostAsync",
            method: "POST",
            data: {
                "contentLink": contents[index].Link,
                "increasePointsIfLiked": increasePoints
            },
            success: callback
        });
    }
});
