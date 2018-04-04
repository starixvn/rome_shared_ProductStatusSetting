class ProductStatusSetting extends ProductStatusSettingBase {
    $: JQueryStatic;
    $enableAllToggles: HTMLElement[];
    $disableAllToggles: HTMLElement[];

    $customerToogles: HTMLElement[];

    $enableToggles: HTMLElement[];
    $disableToggles: HTMLElement[];

    constructor($: JQueryStatic, topWindow: any) {
        super($, topWindow);
    }

    init() {
        super.init();
        this.registerEvents();
    }

    cachedElements() {
        super.cachedElements();

        this.$enableAllToggles = this.$thead.find("span.enabled").toArray() as HTMLElement[];
        this.$disableAllToggles = this.$thead.find("span.disabled").toArray() as HTMLElement[];

        this.$customerToogles = this.$("input[class='checkbox-hide']").toArray() as HTMLElement[];

        this.$enableToggles = this.$tbody.find("span.enabled").toArray() as HTMLElement[];
        this.$disableToggles = this.$tbody.find("span.disabled").toArray() as HTMLElement[];

        this.$resources.enabled = this.$('#enabled').val();
        this.$resources.disabled = this.$('#disabled').val();
        this.$resources.updateActionName = "Update";
    }

    bindingCustomerToggleEvents = () =>
        this.$customerToogles.forEach((toggle) =>
            toggle.addEventListener('click', () => this.toggleCustomer(toggle)));

    bindingEnableDownlineEvents = () =>
        this.$enableToggles.forEach((span) =>
            span.addEventListener('click', () => this.toggleDownline(span)));

    bindingDisableDownlineEvents = () =>
        this.$disableToggles.forEach((span) =>
            span.addEventListener('click', () => this.toggleDownline(span)));

    bindingEnableAllEvents = () =>
        this.$enableAllToggles.forEach((span) =>
            span.addEventListener('click', () => this.toggleStatusAll(span, true)));

    bindingDisableAllEvents = () =>
        this.$disableAllToggles.forEach((span) =>
            span.addEventListener('click', () => this.toggleStatusAll(span, false)));

    registerEvents = () => {
        this.bindingCustomerToggleEvents();
        this.bindingEnableDownlineEvents();
        this.bindingDisableDownlineEvents();
        this.bindingEnableAllEvents();
        this.bindingDisableAllEvents();

        this.bindingResetEvent();
        this.bindingSubmitEvent();

        this.initialHeader();
    }

    initialHeader() {
        for (let roleId = this.$custRole; roleId > 0; roleId--) {
            this.watchHeader(roleId);
        }
    }

    toggleStatusAll = (toggleAll: HTMLElement, status: boolean) => {
        if (this.allowActionOnElement(toggleAll)) {
            let roleId = this.getRoleId(toggleAll);
            this.$custRole == roleId ? this.toggleStatusAllByCustomer(status) : this.toggleStatusAllDownlines(roleId, status);
        }
    }

    toggleStatusAllDownlines = (toggleAllRoleId: number, enable: boolean) => {
        let statusSpanArray = enable ? this.$enableToggles : this.$disableToggles;

        statusSpanArray.forEach((span) => {
            if (this.getRoleId(span) === toggleAllRoleId) {
                span.click();
            }
        });
    }

    toggleStatusAllByCustomer= (enableAll: boolean) =>
        this.$customerToogles.forEach((toggle) => {
            if (this.isChecked(toggle) != enableAll) {
                toggle.click();
            }
        });

    toggleCustomer = (toggle: HTMLElement) => {
        if (this.isEnablebUpline(toggle)) {
            let enable = this.isChecked(toggle);
            let labelElement = this.getNextLabel(toggle);

            let tooltip = this.getTooltip(labelElement, enable);
            let labelText = enable ? this.$resources.enabled : this.$resources.disabled;

            this.$(labelElement).attr("title", tooltip)
                .removeAttr('class')
                .attr('class', labelText.toLowerCase())
                .text(labelText);

            this.toggleStatusDownlines(this.getRoleId(toggle) - 1, enable, this.getBetType(toggle));
        }
    }

    toggleDownline = (toggle: HTMLElement) => {
        if (this.allowActionOnElement(toggle)) {
            let enable = this.isEnabledElement(toggle);
            let roleId = this.getRoleId(toggle);
            let betType = this.getBetType(toggle);

            let oppositeClassName = enable ? this.$resources.disabledClass : this.$resources.enabledClass;
            let oppositeId = this.buildElementId(oppositeClassName, roleId, betType);
            let oppositeStatusSpan = this.$(oppositeId);
            let oppositeTotal = this.parseInt(oppositeStatusSpan.text());

            if (oppositeTotal > 0) {
                oppositeStatusSpan.text("0");
                this.$(toggle).text(this.getTotalStatus(toggle, "total"));
            }

            this.toggleStatusDownlines(roleId - 1, enable, betType);
        }
    }

    watchHeader = (roleId: number) => {
        if (this.$isMultipleBetTypes) {
            this.toogleHeader(roleId);
        }
    }

    isDisableHeader = (toggles: HTMLElement[], isUplineCheckbox: boolean): boolean => 
        toggles.every((toggle) => {
            if (isUplineCheckbox) {
                toggle = this.getNextLabel(toggle);
            }

            return !this.allowActionOnElement(toggle);
        });

    toogleHeader = (roleId: number) => {
        let isCustomer = roleId == this.$custRole;
        let toggles = isCustomer ? this.$customerToogles : this.getToggles(this.$enableToggles, roleId);

        let enabledAllToggle = this.getFirstToggle(this.$enableAllToggles, roleId);
        let disabledAllToggle = this.getFirstToggle(this.$disableAllToggles, roleId);

        if (toggles != null && toggles.length > 0 && this.isDisableHeader(toggles, isCustomer)) {
            this.disableElement(enabledAllToggle);
            this.disableElement(disabledAllToggle);
        }
        else {
            this.enableElement(enabledAllToggle);
            this.enableElement(disabledAllToggle);
        }
    }

    toggleStatusDownlines = (downlineRoleId: number, enable: boolean, betType: number) => {
        if (downlineRoleId > 0) {
            let enabledToggle = this.getDownlineElement(this.$enableToggles, downlineRoleId, betType);
            let disabledToggle = this.getDownlineElement(this.$disableToggles, downlineRoleId, betType);

            if (enable && this.getTotalStatus(enabledToggle, "total") > 0) {
                this.enableElement(enabledToggle);
                this.enableElement(disabledToggle);
            } else {
                this.disableElement(enabledToggle);
                this.disableElement(disabledToggle);
            }

            this.watchHeader(downlineRoleId);

            let currentTotalEnabled = this.$(enabledToggle).text();
            let enableDownline = enable ? (this.parseInt(currentTotalEnabled) > 0) : false;
            this.toggleStatusDownlines(downlineRoleId - 1, enableDownline, betType);
        }
    }

    getDownlineElement = (elements: HTMLElement[], roleId: number, bettype: number): HTMLElement => {
        for (let index = 0; index < elements.length; index++) {
            if (this.getRoleId(elements[index]) == roleId && this.getBetType(elements[index]) == bettype) {
                return elements[index];
            }
        }

        return null;
    }

    getUpdateViewModel() {
        let viewModel = super.getUpdateViewModel();

        let productStatus = this.$('#tbl-product > tbody  > tr').map((index, row) => {
            let firstCheckbox = (this.$(row).find('input:checkbox:first').toArray() as HTMLElement[])[0];
            if (this.isEnablebUpline(firstCheckbox)) {
                let bettype = this.getBetType(firstCheckbox);
                let sporttype = this.getSportType(firstCheckbox);
                let disable = !this.isChecked(firstCheckbox);
                let originstatus = this.parseBoolean(this.$(firstCheckbox).data("originstatus"));

                let data = {
                    sportType: sporttype,
                    betType: bettype,
                    disable: disable,
                    updateAllMasters: false,
                    disableAllMasters: null,
                    updateAllAgents: false,
                    disableAllAgents: null,
                    updateAllMembers: false,
                    disableAllMembers: null
                };

                (this.$(row).find('span:odd').toArray() as HTMLElement[]).forEach((spanElement) => {
                    return this.getProductStatus(spanElement, data);
                });

                if (data.disable != originstatus
                    || data.updateAllMasters
                    || data.updateAllAgents
                    || data.updateAllMembers) {
                    return data;
                }
            }
        }).get();

        viewModel.productStatus = productStatus;
        return viewModel;
    }

    getProductStatus(toggle: HTMLElement, model: any) {
        let levelStatus = this.getLevelStatus(toggle);

        switch (this.getRoleId(toggle)) {
            case 3:
                model.updateAllMasters = levelStatus.UpdateAll;
                model.disableAllMasters = levelStatus.DisableAll;
                break;
            case 2:
                model.updateAllAgents = levelStatus.UpdateAll;
                model.disableAllAgents = levelStatus.DisableAll;
                break;
            case 1:
                model.updateAllMembers = levelStatus.UpdateAll;
                model.disableAllMembers = levelStatus.DisableAll;
                break;
        };

        return model;
    }

    getTotalStatus = (toggle: HTMLElement, customData: string): number => this.$(toggle).data(customData);

    getLevelStatus(toggle: HTMLElement) {
        let totaldisabled = this.parseInt(this.$(toggle).text());
        let originalTotaldisabled = this.getTotalStatus(toggle, "totaldisabled");
        let updateAll = this.allowActionOnElement(toggle) && this.isChangeState(originalTotaldisabled, totaldisabled);

        return {
            UpdateAll: updateAll,
            DisableAll: updateAll && totaldisabled > 0
        }
    }
}

$(function () {
    let productStatusSetting = new ProductStatusSetting($, window.top);
    productStatusSetting.init();
});