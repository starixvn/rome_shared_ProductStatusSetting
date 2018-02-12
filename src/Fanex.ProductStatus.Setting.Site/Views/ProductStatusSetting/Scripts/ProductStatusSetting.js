"use strict";
(function ($, rootUrl, appResources) {
    var enabledSpans, disabledSpans, checkboxHidden, theadEnableSpan, theadDisabledSpan;

    function checkboxChanged() {
        var $this = $(this);
        var labelText = $this.is(":checked") ? appResources.enabled : appResources.disabled;
        toggleStatusDownlines($this);
        $this.next("label")
            .removeClass()
            .addClass(labelText.toLowerCase())
            .text(labelText);
    }

    function toggleStatusDownlines(parent, isEnabled) {
        var checked = false;

        if (typeof (isEnabled) === "undefined")
        {
            checked = parent.is(":checked");
        }
        else {
            checked = isEnabled;
        }

        var roleId = parent.data("roleid");
        var betType = parent.data("bettype");

        if (checked) {
            while (--roleId > 0) {
                if ($("#enabled_" + roleId + "_" + betType).data("total") != 0) {
                    $("#enabled_" + roleId + "_" + betType).removeClass("gray");
                    $("#disabled_" + roleId + "_" + betType).removeClass("gray");
                }
            }
        } else {
            while (--roleId > 0) {
                if ($("#enabled_" + roleId + "_" + betType).data("total") != 0) {
                    $("#enabled_" + roleId + "_" + betType).addClass("gray");
                    $("#disabled_" + roleId + "_" + betType).addClass("gray");
                }
            }
        }

       
        if ($("input:checked").length == 0) {
            while (--roleId01 > 0) {
                if ($("#enabled_" + roleId + "_" + betType).data("total") != 0) {
                    $("#enabled_" + roleId + "_" + betType).removeClass("gray");
                    $("#disabled_" + roleId + "_" + betType).removeClass("gray");
                }
            }
        }
    }


    function toggleStatus(statusSpan, isCheckAll, enableCheckAll) {
        if (!statusSpan.hasClass("gray")) {
            var roleId = statusSpan.data("roleid");
            var betType = statusSpan.data("bettype");

            var className = '';
           // var oppositeClassName = '';
            if (isCheckAll)
            {
                className = enableCheckAll ? "enabled" : "disabled" ;
            }
            else {
                className = statusSpan.attr("class");
            }

            var oppositeClassName = className === "enabled" ? "disabled" : "enabled";
            var oppositeId = ["#", oppositeClassName, "_", roleId, "_", betType].join("");
            var oppositeStatus = $(oppositeId);
            var oppositeTotal = parseInt(oppositeStatus.text().replace(/,/g, ""));

            if (oppositeTotal > 0) {
                oppositeStatus.text("0");
                var currentTotal = parseInt(statusSpan.text().replace(/,/g, ""));
                statusSpan.text((currentTotal + oppositeTotal).toLocaleString());
            }

            toggleStatusDownlines(statusSpan, className === "enabled");
        }
    }

    function toggleAllStatus($this, status) {
       // alert(1);
        //var $this = $(this);

        if (!$this.hasClass("gray")) {
            var statusSpan;
            var roleId = $this.data("roleid");
            //var statusSpanArray = $this.attr("class") === "enabled" ? enabledSpans : disabledSpans;
            var statusSpanArray = status ? enabledSpans : disabledSpans;

            for (var i = 0, max = statusSpanArray.length; i < max; i++) {
                statusSpan = $(statusSpanArray[i]);

                if (statusSpan.data("roleid") === roleId) {
                    toggleStatus(statusSpan, true, status);
                }
            }

            for (var j = 0, max01 = checkboxHidden.length; j < max01; j++) {
                statusSpan = $(checkboxHidden[j]);

                if (statusSpan.data("roleid") === roleId && $(statusSpan).is(":checked") != status) {
                    $(statusSpan).trigger("click");
                }
            }
        }
    }

    function initialize() {
        checkboxHidden = $("input[class='checkbox-hide']:enabled")
            .unbind("change")
            .bind("change", checkboxChanged);

        var table = $("#tbl-product");
        var thead = table.find("thead");
        var tbody = table.find("tbody");
      
        enabledSpans = tbody.find("span.enabled")
            .unbind("click")
            .bind("click", function () {
                toggleStatus($(this));
            });

        disabledSpans = tbody.find("span.disabled")
            .unbind("click")
            .bind("click", function () {
                toggleStatus($(this));
            });

        theadEnableSpan = thead.find("span.enabled")
            .unbind("click")
              .bind("click", function () {
                  toggleAllStatus($(this), true);
              });

          //  .bind("click", toggleAllStatus);

        theadDisabledSpan = thead.find("span.disabled")
            .unbind("click")
         .bind("click", function () {
             toggleAllStatus($(this), false);
         });
            //.bind("click", toggleAllStatus);

        $("#btnReset")
            .unbind("click")
            .bind("click", function () { location.reload(true); });
    }

    function showSuccess(message) {
        showMessage(0, "Update Status Successfully.");
    }

    function showMessage(type, message) {
        var html = ['<div class="', type === 0 ? "alertboxscc" : "alertboxerr", '"><div>', message, "</div></div>"].join("");
        var messageDiv = $("#sectionMessage");
        return messageDiv.html(html).removeClass("hide");
    }

    function hideMessage() {
        var messageDiv = $("#sectionMessage");
        return messageDiv.html("").addClass("hide");
    }

    initialize();
})(jQuery, rootUrl, appResources);