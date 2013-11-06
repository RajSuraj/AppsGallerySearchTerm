Feature: SearchAppName
Search applications for terms that has the expected name

Scenario: search applications for terms has expected name
    Given the alteryx service is running at "http://gallery.alteryx.com"
    When I invoke GET at application details at "api/apps/gallery" with search term "choosing"
    Then I see primaryapplication.metainfo.name contains "Site Selection Demo"
