/// <reference path="jquery-3.3.1.js" />
/// <reference path="//www.instagram.com/embed.js"/>
let points = 0;
let currentContentIndex = 0;
let numberOfFollowsBefore = 0;
let numberOfFollowsAfter = 0;
let contents = [];
let pointsLabel = $('#points');
let cards = $('.multiple-items');
function IncreasePointsIfFollowed(index) {
    if (contents[index].counts.followed_by < numberOfFollowsAfter) {
        points = points + 10;
        pointsLabel.text(points);
        cards.slick('slickRemove', index);
    }
}

async function getNumberOfFollows(index, isBefore, callback) {
    $.ajax({
        url: "GetNumberOfFollows",
        data: {
            "id": contents[index].id
        },
        success: function (response) {
            console.log(response);
            if (isBefore) {
                numberOfFollowsBefore = response;
            }
            else {
                numberOfFollowsAfter = response;
            }
            if (callback) {
                callback(index);
            }
        }
    });
}
function openFollowWindow(source) {
    let popup = window.open($(source).data('href'), '_blank', 'toolbar=yes,scrollbars=yes,resizable=yes,top=500,left=500,width=400,height=400');
    var timer = setInterval(function () {
        if (popup.closed) {
            clearInterval(timer);
            getNumberOfFollows($(source).data('customindex'), false, IncreasePointsIfFollowed);
        }
    }, 1000);
}
$(document).ready(function () {
    $(window).resize(function () {
        cards.slick('resize');
    });
    cards.slick({
        slidesToShow: 4,
        slidesToScroll: 2,
        adaptiveHeight: false,
        infinite: true,
        responsive: [
            {
                breakpoint: 1024,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 3,
                    infinite: true,
                    dots: true
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ]

    });
    getContent(true);

    function getContent(isFirst) {
        $.ajax({
            url: 'GetAccounts',
            data: {
                "order": contents.length / 10
            },
            success: function (response) {
                console.log(response);
                index = contents.length;
                currentContentIndex = contents.length;
                $.each(response, function () {
                    contents.push(this);
                    if (isFirst) {
                        numberOfFollowsBefore = this.counts.followed_by;
                        setContent(index);
                    }
                    index = index + 1;
                });
                $('#loading').hide();
            }
        });
    }

    function setContent(index) {
        let card = `<div><div><div class="card">
                <div class="card-header">
                    `+ contents[index].full_name + `
                </div>
                <div class="card-body ">
                    <img class="card-img img-fluid" style="max-width: 180px; max-height: 180px" src="`+ contents[index].profile_picture + `" />
                    @`+ contents[index].username + `
                </div>
                <div class="card-footer">
                    <h6>`+ contents[index].counts.media + ` post   </h6>
                    <h6>` + contents[index].counts.followed_by + ` followers</h6>
                    <input onclick="openFollowWindow(this)" data-customindex="`+ index + `" type="button" class="follow-button btn btn-primary" data-href="https://instagram.com` + '/' + contents[index].username + `" value="Follow"/>
                </div>
            </div></div></div>`;
        cards.slick('slickAdd', card);
    }
});