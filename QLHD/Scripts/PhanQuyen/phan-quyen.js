jQuery(document).ready(function () {
    var menu, users;
    // Users
    // Generate users trees
    $("#tree_users").fancytree({
        checkbox: true,
        selectMode: 3,
        folder: false,
        extensions: ["filter"],
        quicksearch: true,
        source: { url: "/HeThong/DanhSachUsers" },
        select: function (event, data) {
            users = $.map(data.tree.getSelectedNodes(), function (node) {
                return node.key;
            });
        },
        filter: {
            autoApply: true,
            //autoExpand: true,
            mode: "hide"
        }
    });

    //xem quyen function
    jQuery(document).on("click", "#xem_quyen", function () {
        $(".result-status").html("");
        $("#tree_quyen").fancytree("getTree").visit(function (node) {
            node.setSelected(false);
        });
        if (users == null || users.length == 0) {
            VNPT.Common.Notify("Vui lòng chọn tài khoản cần phân quyền", "danger");
        } else {
            $.ajax({
                url: "/PhanQuyen/XemQuyen",
                type: "POST",
                traditional: true,
                data: { list_user: users },
                dataType: "json",
                error: function (xhr, status, errmgs) {

                },
                beforeSend: function () {
                    $(".loader-container").addClass("active");
                },
                complete: function () {
                    $(".loader-container").removeClass("active");
                },
                success: function (result) {
                    $.each(result, function (i, field) {
                        $("#tree_quyen").fancytree("getTree").visit(function (node) {
                            if (node.key == field)
                                node.setSelected(true);
                        });
                    });
                }
            });
        }
    });

    //Generate quyen trees
    $("#tree_quyen").fancytree({
        checkbox: true,
        selectMode: 2,
        folder: false,
        extensions: ["filter"],
        quicksearch: true,
        source: { url: "/PhanQuyen/DanhSachQuyen" },
        select: function (event, data) {
            menu = $.map(data.tree.getSelectedNodes(), function (node) {
                return node.key;
            });
        },
        filter: {
            autoApply: true,
            //autoExpand: true,
            mode: "hide"
        }
    });

    //Select all quyen
    $("#check_all_quyen").change(function () {
        if ($("#check_all_quyen").is(":checked")) {
            $("#tree_quyen").fancytree("getTree").visit(function (node) {
                node.setSelected(true);
            });
        }
        else {
            $("#tree_quyen").fancytree("getTree").visit(function (node) {
                node.setSelected(false);
            });
        }
        return false;
    });

    //Update Quyen
    jQuery(document).on("click", "#capnhat_quyen", function () {
        if (users == null || users.length == 0) {
            VNPT.Common.Notify("Vui lòng chọn tài khoản cần phân quyền", "danger");
        }
        else {
            $.ajax({
                url: "/PhanQuyen/CapNhatQuyen",
                type: "POST",
                traditional: true,
                data: { list_user: users, list_quyen: menu },
                dataType: "json",
                error: function (xhr, status, errmgs) {

                },
                beforeSend: function () {
                    $(".loader-container").addClass("active");
                },
                complete: function () {
                    $(".loader-container").removeClass("active");
                },
                success: function (result) {
                    if (result.Code == 200) {
                        VNPT.Common.Notify(result.Message, "success");
                    } else {
                        VNPT.Common.Notify(result.Message, "danger");
                    }
                }
            });
        }
    });


});