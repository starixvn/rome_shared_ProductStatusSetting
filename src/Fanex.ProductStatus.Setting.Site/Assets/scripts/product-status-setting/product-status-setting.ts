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

    toggleStatusAllByCustomer = (enableAll: boolean) =>
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

            this.toggleStatusDownlines(this.getRoleId(toggle) - 1, enable, this.getSportType(toggle), this.getBetType(toggle));
        }
    }

    toggleDownline = (toggle: HTMLElement) => {
        if (this.allowActionOnElement(toggle)) {
            let enable = this.isEnabledElement(toggle);
            let roleId = this.getRoleId(toggle);
            let betType = this.getBetType(toggle);
            let sportType = this.getSportType(toggle);
            let oppositeClassName = enable ? this.$resources.disabledClass : this.$resources.enabledClass;
            let oppositeId = this.buildElementId(oppositeClassName, roleId, sportType, betType);
            let oppositeStatusSpan = this.$(oppositeId);
            let oppositeTotal = this.parseInt(oppositeStatusSpan.text());

            if (oppositeTotal > 0) {
                oppositeStatusSpan.text("0");
                this.$(toggle).text(this.getTotalStatus(toggle, "total"));
            }
             
            this.toggleStatusDownlines(roleId - 1, enable, sportType, betType);
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

    toggleStatusDownlines = (downlineRoleId: number, enable: boolean, sportType: number, betType: number, totalDisabledUpline: number = 0) => {
        if (downlineRoleId > 0) {
            let enabledToggle = this.getDownlineElement(this.$enableToggles, downlineRoleId, sportType, betType);
            let disabledToggle = this.getDownlineElement(this.$disableToggles, downlineRoleId,sportType, betType);
            let total = this.getTotalStatus(enabledToggle, "total");

            if (enable && total > 0) {
                totalDisabledUpline == 0 ? this.enableElement(enabledToggle) : this.disableElement(enabledToggle);
                this.enableElement(disabledToggle);
            } else {
                this.disableElement(enabledToggle);
                this.disableElement(disabledToggle);

                this.$(enabledToggle).text("0");
                this.$(disabledToggle).text(total);
            }

            this.watchHeader(downlineRoleId);

            let currentTotalEnabled = this.$(enabledToggle).text();
            let enableDownline = enable ? (this.parseInt(currentTotalEnabled) > 0) : false;
            let currentTotalDisabled = this.parseInt(this.$(disabledToggle).text());

            this.toggleStatusDownlines(downlineRoleId - 1, enableDownline, sportType, betType, currentTotalDisabled);
        }
    }

    getDownlineElement = (elements: HTMLElement[], roleId: number, sportType: number, bettype: number): HTMLElement => {
        for (let index = 0; index < elements.length; index++) {
            if (this.getRoleId(elements[index]) == roleId && this.getSportType(elements[index]) == sportType && this.getBetType(elements[index]) == bettype) {
                return elements[index];
            }
        }

        return null;
    }

    getUpdateViewModel() {
        let viewModel = super.getUpdateViewModel();
        viewModel.productStatus = this.getProductStatus();
        return viewModel;
    }

    getProductStatus()
    {
        return this.$('#tbl-product > tbody  > tr').map((index, row) => {
            let customerElement = (this.$(row).find('input:checkbox:first').toArray() as HTMLElement[])[0];

            let disableCustomer = !this.isChecked(customerElement);
            let originDisableCustomer = this.parseBoolean(this.$(customerElement).data("originstatus"));
            let updateCustomerStatus = !disableCustomer || !originDisableCustomer;

            if (this.isEnablebUpline(customerElement) && updateCustomerStatus) {
                let bettype = this.getBetType(customerElement);
                let sporttype = this.getSportType(customerElement);

                let data = {
                    sportType: sporttype,
                    betType: bettype,
                    disable: disableCustomer,
                    updateAllMasters: false,
                    disableAllMasters: false,
                    updateAllAgents: false,
                    disableAllAgents: false,
                    updateAllMembers: false,
                    disableAllMembers: false
                };


                (this.$(row).find('span:odd').toArray() as HTMLElement[]).forEach((spanElement) => {
                    return this.getDownlineStatus(spanElement, data);
                });

                if (data.disable != originDisableCustomer
                    || data.updateAllMasters
                    || data.updateAllAgents
                    || data.updateAllMembers) {
                    return data;
                }
            }
        }).get();
    }

    getDownlineStatus(toggle: HTMLElement, data: any) {
        switch (this.getRoleId(toggle)) {
            case 3:
                let masterStatus = this.getLevelStatus(toggle, data.disable);
                data.updateAllMasters = masterStatus.UpdateAll;
                data.disableAllMasters = masterStatus.DisableAll;
                break;

            case 2:
                let agentStatus = this.getLevelStatus(toggle, this.$custRole == 3 ? data.disable : data.disableAllMasters);
                data.updateAllAgents = agentStatus.UpdateAll;
                data.disableAllAgents = agentStatus.DisableAll;
                break;

            case 1:
                let memberStatus = this.getLevelStatus(toggle, this.$custRole == 2 ? data.disable : data.disableAllAgents);
                data.updateAllMembers = memberStatus.UpdateAll;
                data.disableAllMembers = memberStatus.DisableAll;
                break;
        };

        return data;
    }

    getTotalStatus = (toggle: HTMLElement, customData: string): number => this.$(toggle).data(customData);

    getLevelStatus(toggle: HTMLElement, disabledUpline: boolean) {
        if (disabledUpline)
        {
            return {
                UpdateAll: true,
                DisableAll: true
            }
        }

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