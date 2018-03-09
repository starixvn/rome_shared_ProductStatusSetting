describe("jasmine.Fixtures", function () {
    var ajaxData = 'some ajax data'
    var fixtureUrl = 'some_url'
    var anotherFixtureUrl = 'another_url'
    var fixturesContainer = function () {
        return $('#' + jasmine.getFixtures().containerId)
    }

    var appendFixturesContainerToDom = function () {
        $('body').append('<div id="' + jasmine.getFixtures().containerId + '">old content</div>')
    }

    beforeEach(function () {
        jasmine.getFixtures().clearCache()
        spyOn(jasmine.getFixtures(), 'loadFixtureIntoCache_').and.callFake(function (relativeUrl) {
            this.fixturesCache_[relativeUrl] = ajaxData
        })
    })

    describe("default initial config values", function () {
        it("should set 'jasmine-fixtures' as the default container id", function () {
            expect(jasmine.getFixtures().containerId).toEqual('jasmine-fixtures')
        })

        it("should set 'spec/javascripts/fixtures' as the default fixtures path", function () {
            expect(jasmine.getFixtures().fixturesPath).toEqual('spec/javascripts/fixtures')
        })
    })
});