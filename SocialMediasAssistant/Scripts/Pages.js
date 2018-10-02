let lateContentLikes = 0;
let currentHasLiked = false;
let points = 0;
let currentContentIndex = 0;
let numberOfLikesBefore = 0;
let numberOfLikesAfter = 0;
let contents = [];
$(document).ready(function () {
    window.fbAsyncInit = function () {
        FB.init({
            appId: '894196017436373',
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
    function getContent(isFirst) {
        $.ajax({
            url: 'GetPages',
            data: {
                "order": contents.length / 10
            },
            success: function (response) {
                console.log(response);
                $.each(response, function () {
                    contents.push(this);
                });
                if (isFirst) {
                    getNumberOfLikes(currentContentIndex, true, setContent);
                }
            }
        });
    }
    let cards = $('#cards');
    let pointsLabel = $('#points');
    function setContent() {
        pointsLabel.text(points);
        let card = `<div class="fb-page" data-href="` + contents[currentContentIndex].Link + `" data-tabs="timeline" data-small-header="false" data-adapt-container-width="true" data-hide-cover="false" data-show-facepile="true">
                        <blockquote cite="`+ contents[currentContentIndex].Link + `" class="fb-xfbml-parse-ignore">
                            <a href="`+ contents[currentContentIndex].Link + `">
                                Some Page
                            </a>
                        </blockquote>
                    </div>
`;
        cards.html(card);
        FB.XFBML.parse();
        getNumberOfLikes(currentContentIndex, true, null);
    }

    $('#refresh').click(function () {
        getNumberOfLikes(currentContentIndex, false, IncreasePointsIfLiked);
        currentContentIndex = currentContentIndex + 1;
    });
    function IncreasePointsIfLiked() {
        if (numberOfLikesBefore < numberOfLikesAfter) {
            points = points + 10;
        }
        setContent();
    }

    async function getNumberOfLikes(index, isBefore, callback) {
        $.ajax({
            url: "GetNumberOfLikes",
            data: {
                "link": contents[index].Link,
                "numberOfPrevLikes": numberOfLikesBefore,
                "increasePoints": !isBefore
            },
            success: function (response) {
                console.log(response);
                if (isBefore) {
                    numberOfLikesBefore = response;
                }
                else {
                    numberOfLikesAfter = response;
                }
                if (callback) {
                    callback();
                }
            }
        });
    }
});