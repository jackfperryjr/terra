
$(document).ready(function(){
    setTimeout(function(){ 
        $("#overlay").fadeOut(250);
    }, 1500);

    $("#login-btn").off().on("click", function() {
        $("#loader").show();
    });
});

$(this).delay(1000).queue(function() {
    let search = $('#search').offset().top;

    let stickySearch = function(){
        let scrollTop = $(window).scrollTop();
    
        if (scrollTop > search) { 
            $("#search").addClass("sticky-search-bar");
            $("#search-icon").addClass("sticky-search-icon")
        } else {
            $('#search').removeClass("sticky-search-bar"); 
            $("#search-icon").removeClass("sticky-search-icon")
        }
    };

    stickySearch();
    $(window).scroll(function() {
        stickySearch();
    });
});

function hideSuccess() {
    $("#edit-success-container").hide();
}

$("#img-input1").change(function(event){
    let imgPath = URL.createObjectURL(event.target.files[0]);
    $("#img-output1").fadeIn("fast").attr('src',imgPath); 
});
$("#img-input2").change(function(event){
    let imgPath = URL.createObjectURL(event.target.files[0]);
    $("#img-output2").fadeIn("fast").attr('src',imgPath); 
});
$("#img-input3").change(function(event){
    let imgPath = URL.createObjectURL(event.target.files[0]);
    $("#img-output3").fadeIn("fast").attr('src',imgPath); 
});
$("#img-input4").change(function(event){
    let imgPath = URL.createObjectURL(event.target.files[0]);
    $("#img-output4").fadeIn("fast").attr('src',imgPath); 
});
$("#img-input5").change(function(event){
    let imgPath = URL.createObjectURL(event.target.files[0]);
    $("#img-output5").fadeIn("fast").attr('src',imgPath); 
});