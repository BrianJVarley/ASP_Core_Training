//site.js

//anonmymous function to prevent naming coolisions with
//other JS files
(function () {


    //var ele = $("#username");
    //ele.text("Brian Varley");

    //var main = $("#main");

    //main.on("mouseenter", function () {
    //    main.style = "background-color: #888;";


    //});

    //main.on("mouseleave", function () {

    //    main.style = "";

    //});

    //var menuItems = $("ul.menu li a");
    //menuItems.on("click", function () {
    //    var clicked = $(this);
    //    alert(clicked.text());
    //});


    var $sidebarAndWrapper = $("#sidebar, #wrapper-main");

    $("#sidebarToggle").on("click", function () {

        $sidebarAndWrapper.toggleClass("hide-sidebar");

        if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
            $(this).text("Show Menu");

        }
        else {

            $(this).text("Hide Menu");
        }
    });





})();
