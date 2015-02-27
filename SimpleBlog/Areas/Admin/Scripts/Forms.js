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

    $("[data-slug]").each(function() {
        var $this = $(this);
        var $sendSlugFrom = $($this.data("slug"));

        $sendSlugFrom.keyup(function() {   //everytime a key is pressed
            var slug = $sendSlugFrom.val();
            slug = slug.replace(/[^a-zA-Z0-9\s]/g, "");  //replace special characters with an empty string
            slug = slug.toLowerCase();
            slug = slug.replace(/\s+/g, "-");  //replace one or more spaces with a dash

            if (slug.charAt(slug.length - 1) == "-")  //trim final character if this is a dash
                slug = slug.substr(0, slug.length - 1);

            $this.val(slug);

        });


    });
});