var ProductStatusSetting = (function () {
    function ProductStatusSetting($) {
        var _this = this;
        this.resources = {
            enabled: 'Enabled',
            disabled: 'Disabled'
        };
        this.init = function () {
            _this.cachedElements();
            _this.registerEvents();
        };
        this.cachedElements = function () {
            var self = _this;
            var table = self.$("#tbl-product");
            var thead = table.find("thead");
            var tbody = table.find("tbody");
            _this.$checkboxHidden = self.$("input[class='checkbox-hide']");
            _this.$enabledSpans = tbody.find("span.enabled");
            _this.$disabledSpans = tbody.find("span.disabled");
            _this.$theadEnableSpan = thead.find("span.enabled");
            _this.$theadDisabledSpan = thead.find("span.disabled");
        };
        this.checkboxChanged = function ($this) {
            var labelText = _this.$($this).is(":checked") ? _this.resources.enabled : _this.resources.disabled;
            _this.toggleStatusDownlines($this);
            $this.next("label")
                .removeClass()
                .addClass(labelText.toLowerCase())
                .text(labelText);
        };
        this.toggleStatusDownlines = function ($this, isEnabled) {
            var checked = false;
            if (typeof (isEnabled) === "undefined" || isEnabled == null) {
                checked = _this.$($this).is(":checked");
            }
            else {
                checked = isEnabled;
            }
            // let parent = $this.closet('tr');
            var roleId = $this.data("roleid");
            var roleId01 = $this.data("roleid");
            var betType = $this.data("bettype");
            if (checked) {
                while (--roleId > 0) {
                    if (_this.$("#enabled_" + roleId + "_" + betType).data("total") != 0) {
                        _this.$("#enabled_" + roleId + "_" + betType).removeClass("gray");
                        _this.$("#disabled_" + roleId + "_" + betType).removeClass("gray");
                    }
                }
            }
            else {
                while (--roleId > 0) {
                    if (_this.$("#enabled_" + roleId + "_" + betType).data("total") != 0) {
                        _this.$("#enabled_" + roleId + "_" + betType).addClass("gray");
                        _this.$("#disabled_" + roleId + "_" + betType).addClass("gray");
                    }
                }
            }
            if (_this.$("input:checked").length == 0) {
                while (--roleId01 > 0) {
                    if (_this.$("#enabled_" + roleId + "_" + betType).data("total") != 0) {
                        _this.$("#enabled_" + roleId + "_" + betType).removeClass("gray");
                        _this.$("#disabled_" + roleId + "_" + betType).removeClass("gray");
                    }
                }
            }
        };
        this.toggleStatus = function (statusSpan, isCheckAll, enableCheckAll) {
            if (!statusSpan.hasClass("gray")) {
                var roleId = statusSpan.data("roleid");
                var betType = statusSpan.data("bettype");
                var className = '';
                // var oppositeClassName = '';
                if (isCheckAll) {
                    className = enableCheckAll ? "enabled" : "disabled";
                }
                else {
                    className = statusSpan.attr("class");
                }
                var oppositeClassName = className === "enabled" ? "disabled" : "enabled";
                var oppositeId = ["#", oppositeClassName, "_", roleId, "_", betType].join("");
                var oppositeStatus = _this.$(oppositeId);
                var oppositeTotal = parseInt(oppositeStatus.text().replace(/,/g, ""));
                if (oppositeTotal > 0) {
                    oppositeStatus.text("0");
                    var currentTotal = parseInt(statusSpan.text().replace(/,/g, ""));
                    statusSpan.text((currentTotal + oppositeTotal).toLocaleString());
                }
                _this.toggleStatusDownlines(statusSpan, className === "enabled");
            }
        };
        this.toggleAllStatus = function ($this, status) {
            var self = _this;
            if (!$this.hasClass("gray")) {
                var statusSpan = void 0;
                var roleId = $this.data("roleid");
                //var statusSpanArray = $this.attr("class") === "enabled" ? enabledSpans : disabledSpans;
                var statusSpanArray = status ? self.$enabledSpans : self.$disabledSpans;
                for (var i = 0, max = statusSpanArray.length; i < max; i++) {
                    statusSpan = self.$(statusSpanArray[i]);
                    if (statusSpan.data("roleid") === roleId) {
                        self.toggleStatus(statusSpan, true, status);
                    }
                }
                for (var j = 0, max01 = self.$checkboxHidden.length; j < max01; j++) {
                    statusSpan = self.$(self.$checkboxHidden[j]);
                    if (statusSpan.data("roleid") === roleId && self.$(statusSpan).is(":checked") != status) {
                        statusSpan.click();
                    }
                }
            }
        };
        this.registerEvents = function () {
            var self = _this;
            self.$checkboxHidden.each(function (index, el) {
                self.$(el).bind("change", function () {
                    self.checkboxChanged(self.$(this));
                });
            });
            self.$enabledSpans.each(function (index, el) {
                self.$(el).bind("click", function () {
                    self.toggleStatus(self.$(this));
                });
            });
            self.$disabledSpans.each(function (index, el) {
                self.$(el).bind("click", function () {
                    self.toggleStatus(self.$(this));
                });
            });
            self.$theadEnableSpan.each(function (index, el) {
                self.$(el).bind("click", function () {
                    self.toggleAllStatus(self.$(this), true);
                });
            });
            self.$theadDisabledSpan.each(function (index, el) {
                self.$(el).bind("click", function () {
                    self.toggleAllStatus(self.$(this), false);
                });
            });
            self.$("#btnReset")
                .unbind("click")
                .bind("click", function () { location.reload(true); });
            self.$("#btnSubmit")
                .unbind("click")
                .bind("click", self.onSubmit);
        };
        this.onSubmit = function () {
        };
        this.onComplete = function () {
            _this.$.unblockUI();
        };
        this.$ = $;
    }
    return ProductStatusSetting;
}());
var productStatusSetting = new ProductStatusSetting($);
$(function () {
    productStatusSetting.init();
});
