describe('ProductStatusSetting_', () => {
    const productStatusSetting = new ProductStatusSetting($);

    // Setup
    beforeEach(function () {
        jasmine.getFixtures().fixturesPath = 'fixtures/';
        jasmine.getFixtures().load('numbergame.html');
    });

    describe("parseInt_", () => {
        it("Should return correct value", function () {
            expect(productStatusSetting.parseInt("5")).toEqual(5);
        });
    });

    describe("parseBoolean_", () => {
        it("Should return correct value", function () {
            expect(productStatusSetting.parseBoolean("True")).toBeTruthy();
            expect(productStatusSetting.parseBoolean("true")).toBeTruthy();

            expect(productStatusSetting.parseBoolean("False")).toBeFalsy();
            expect(productStatusSetting.parseBoolean("false")).toBeFalsy();
        });
    });

    describe("buildElementId_", () => {
        it("Should return correct Element Id", function () {
            // Act
            let elementId = productStatusSetting.buildElementId("enabled", 3, 12);

             // Assert
            expect(elementId).toEqual("#enabled_3_12");
        });
    });

    describe("getBetType_", () => {
        it("Should return correct bettype id", function () {
            // Act
            let elementId = productStatusSetting.buildElementId("enabled", 3, 81);
            let actualSporttype = productStatusSetting.getBetType($(elementId));

            // Assert
            expect(actualSporttype).toEqual(81);
        });
    });

    describe("getSportType_", () => {
        it("Should return correct sporttype id", function () {
            // Act
            let elementId = productStatusSetting.buildElementId("enabled", 3, 81);
            let actualBettype = productStatusSetting.getSportType($(elementId));

            // Assert
            expect(actualBettype).toEqual(161);
        });
    });

    describe("getTotalStatus_", () => {
        it("Should return correct total statuas", function () {
            // Act
            let elementId = productStatusSetting.buildElementId("enabled", 3, 81);
            let actualTotalStatus = productStatusSetting.getTotalStatus($(elementId));

            // Assert
            expect(actualTotalStatus).toEqual(615);
        });
    });

    describe("isChangeState_", () => {
        it("Should return change state of element", function () {
            expect(productStatusSetting.isChangeState(5, 7)).toBeTruthy();
            expect(productStatusSetting.isChangeState(5, 5)).toBeFalsy();
        });
    });
});