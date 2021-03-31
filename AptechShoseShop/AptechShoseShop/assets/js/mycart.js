$.cookie.json = true;
$.cookie.defaults.path = '/';
//var arrAjax = new Object();

function getCartItems() {
    if ($.cookie('productlist')) {
        return $.cookie('productlist').cartItems;
    } else {
        return [];
    }
}


function saveCartItems(cart_items) {

    var obj = { "cartItems": cart_items };
    $.cookie('productlist', obj, { expires: 30 }); //expires = 30 days
}
function emptyCartItems() {
    $.removeCookie('productlist');
}



function addCart(productId) {
    $(document).ready(function () {   
        var colorName = document.getElementById("ColorName");
        var colorSelected = colorName.options[colorName.selectedIndex].value;
        if (colorSelected === "#") {
            alert("Bạn chưa chọn màu");
            return;
        };

        var sizeName = document.getElementById("SizeName");
        var sizeSelected = sizeName.options[sizeName.selectedIndex].value;
        if (sizeSelected === "#") {
            alert("Bạn chưa chọn size");
            return;
        };

        var quantity = $("#quantity").val();      
        var cart_items = getCartItems();
      
        var is_exist = false;
        $(cart_items).each(function (i, v) {
            if (v && v.productid === productId) {
                ///kiểm tra thêm màu và size
                is_exist = true;
            }
        });
        
        if (!is_exist) {
            var new_item = {
                "productid": productId,
                "quantity": quantity,
                "ColorName": colorSelected,
                "SizeName": sizeSelected
            };
            cart_items.push(new_item);
            saveCartItems(cart_items);
            swal({
                text: "Bạn đã thêm sản phẩm này vào giỏ hàng.",
                icon: "success",
                buttons: {
                    cancel: "Cancel",
                    catch: {
                        text: "Proceed to checkout",
                        value: "cart"
                    }
                }
            }).then((value) => {
                switch (value) {
                    case "cart":
                        location.href = "/cart";
                        break;
                }
            });
        }
        else {
            swal("Notification", "Sản phẩm này đã tồn tại trong giỏ.!", "error");
          
        }
        console.log("ds");
    });
   
 
}


function bindingCart() {
    $(document).ready(function () {
        var cart_items = getCartItems();
        $.ajax({
            type: "GET",
            url: '/Cart/Binding',
            data: "data=" + JSON.stringify(cart_items),
            success: function (response) {
                $("#binding-cart").html(response);


            },
            error: function (ex) {

                console.log("Error!!" + ex);
            }

        });


    });
}


function updateItem(productid, q) {
    productid = productid.toString();
    q = q.toString();
    ///console.log(q);
    var cart_items = getCartItems();

    $(cart_items).each(function (i, v) {
        if ((v && v.productid === productid)
        ) {
            cart_items[i].quantity = q;
            saveCartItems(cart_items);
            bindingCart();
        }
    });

}






function removeItem(id) {
    swal({
        title: "Xác nhận xóa.",
        text: "Xóa sản phẩm này khỏi giỏ hàng",
        icon: "warning",
        buttons: true,
        dangerMode: true
    })
        .then((isConfirm) => {
            if (isConfirm) {
                //  id = id.toString();
                var cart_items = getCartItems();
                for (var i = 0; i < cart_items.length; i++) {
                    if (cart_items[i].productid === id) {

                        cart_items.splice(i, 1);
                    }
                }
                saveCartItems(cart_items);
                bindingCart();

            } else {
                // nếu ấn cancel
            }
        });


}




function Checkout() {
    $(document).ready(function () {

        var cart_items = getCartItems();
        var CustomerName = $("#fullName").val();
        var CustomerPhone = $("#phone").val();
        var CustomerAddress = $("#address").val();
        var CustomerEmail = $("#email").val();
        var OrderNote = $("#orderNote").val();
      

        $.ajax({
            url: '/Checkout/Checkout',
            type: "POST",

            data: "data=" + JSON.stringify(cart_items) + "&CustomerName=" + CustomerName
                + "&CustomerPhone=" + CustomerPhone
                + "&CustomerEmail=" + CustomerEmail
                + "&CustomerAddress=" + CustomerAddress
            
                + "&OrderNote=" + OrderNote
          ,
            success: function (response) {
                if (response === "OK") {
                    swal("Thông báo !", "Bạn đã đặt hàng thành công.", "success");
                    emptyCartItems();
                   
                }
                else {
                    swal(response);
                }
            },
            error: function () {
                console.log("Lỗi");
            }
        });
    });

}
//





