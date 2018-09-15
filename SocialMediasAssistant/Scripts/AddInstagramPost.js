
$(document).ready(function () {
    $.ajax({
        url: 'GetCurrentUserPosts',
        success: function (response) {
            console.log(response);
            let posts = $('#posts div .row');
            $.each(response.data, function (indexPost, value) {
                let link = value.link;
                setContent(link, posts);
            });
            instgrm.Embeds.process();
        }
    });
    let postLink = $('#postLink');
    $('#postsModal').on('hidden.bs.modal', function (e) {
        let link = $('input[name=postLinks]:checked').val();
        postLink.val(link);
        postLink.text(link);
    });
});
function setContent(link, posts) {
    let post = `<label class="btn btn-light col-md-12">
                   <input type="radio" name="postLinks" autocomplete="off" value="`+ link + `"> 
                    <div class="card"><div class= "card-body justify-content-center d-flex" style="padding:2%">
            <blockquote class="instagram-media" data-instgrm-permalink="`+link+`?utm_source=ig_embed" data-instgrm-version="9" style=" background:#FFF; border:0; border-radius:3px; box-shadow:0 0 1px 0 rgba(0,0,0,0.5),0 1px 10px 0 rgba(0,0,0,0.15); margin: 1px; max-width:540px; min-width:326px; padding:0; width:99.375%; width:-webkit-calc(100% - 2px); width:calc(100% - 2px);">
                                <div style="padding:8px;">
                                    <a href="`+link+`?utm_source=ig_embed" style=" color:#c9c8cd; font-family:Arial,sans-serif; font-size:14px; font-style:normal; font-weight:normal; line-height:17px; text-decoration:none;" target="_blank">A post shared by Rewid Vezirov (@@rewid_vezirov)</a>
                                </div>
                            </blockquote></div></div>
               </label>
                <hr/>`;
    posts.append(post);
}