$(document).ready(function () {

    $("a[data-post]").click(function (e) {
        e.preventDefault();

        var $this = $(this);
        var message = $this.data("post");

        if (message && !confirm(message))
            return;

        //took this code out because I was getting an error I could not resolve.  moving on for now.  come back to this later. 
        //this is from lecture 20   once i uncomment these lines, I will also need to
        //1  go to areas/admin/scripts.forms.js and uncomment the hidden form at the bottom of that page
        //2 go to UserController.cs and add the 'ValidateAntiForgeryToken' back to the delete function
      //  var antiForgeryToken = $("#anti-forgery-form input");
       // var antiForgeryInput = $("<input type='hidden'").attr("name", antiForgeryToken.attr("name")).val(antiForgeryToken.val());

        $("<form>")
        .attr("method", "post")
        .attr("action", $this.attr("href"))
        //.appendTo(antiForgeryInput)
        .appendTo(document.body)
        .submit();
    });
});