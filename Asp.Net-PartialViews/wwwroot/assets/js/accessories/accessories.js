


$(document).ready(function () {

    $(document).on("click", ".btn-primary", function () {
      

        let parentProducts = $("#parent-products");

        let skipCount = $("#parent-products").children().length;

        let productCount = $("#product-count").val();

    
        $.ajax({
            url: "/accessories/loadmore",
            type: "Get",
            data: {
                skip: skipCount
            },
            success: function (resp) {

                $(parentProducts).append(resp);
                skipCount = $("#parent-products").children().length;
               
                if (skipCount >= productCount) {
                    $(".btn-primary").addClass("d-none");
                }
            }
        })

     
       
    })
    $(document).on("submit", "form", function () {
        $.ajax({
            url: "/accessories/addbasket",
            type: "Post",
            success: function (response) {
                console.log("ok")
                if (response.success) {
                    alert("ss")
                }
            }

        })
    });

})