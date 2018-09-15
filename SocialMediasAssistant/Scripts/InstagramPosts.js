/// <reference path="jquery-3.3.1.js" />
/// <reference path="//www.instagram.com/embed.js"/>
let lateContentLikes = 0;
let currentHasLiked = false;
let points = 0;
let currentContentIndex = 0;
let numberOfLikesBefore = 0;
let numberOfLikesAfter = 0;
let contents = [];
$(document).ready(function () {
    getContent(true);


    function getContent(isFirst) {
        $.ajax({
            url: 'GetPosts',
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
        let card = `<blockquote class="instagram-media" data-instgrm-permalink="`+ contents[currentContentIndex].Link + `?utm_source=ig_embed" data-instgrm-version="9" style=" background:#FFF; border:0; border-radius:3px; box-shadow:0 0 1px 0 rgba(0,0,0,0.5),0 1px 10px 0 rgba(0,0,0,0.15); margin: 1px; max-width:540px; min-width:326px; padding:0; width:99.375%; width:-webkit-calc(100% - 2px); width:calc(100% - 2px);">
                                <div style="padding:8px;">
                                    <a href="`+ contents[currentContentIndex].Link + `?utm_source=ig_embed" style=" color:#c9c8cd; font-family:Arial,sans-serif; font-size:14px; font-style:normal; font-weight:normal; line-height:17px; text-decoration:none;" target="_blank">A post shared by Rewid Vezirov (@@rewid_vezirov)</a>
                                </div>
                            </blockquote>`;
        cards.html(card);
        
        getNumberOfLikes(currentContentIndex, true, null);
        instgrm.Embeds.process();
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
            url: "GetPostLikes",
            data: {
                "link":contents[index].Link
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