# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.2.6] - 2022-06-10
### Added
- Added AsyncUniversalEventManager for asynchronous events.

## [0.2.5] - 2022-05-31
### Added
- Added GlobalEventSearcher. Support basic search feature across all global events.
- Added event copy feature.
- Added event paste feature.
- Support locating the object when find the event reference.

### Fixed
- Set the settings dirty when make change to the events to save event reference record.

## [0.2.4] - 2022-02-02
### Changed
- Ask the developer whether to add the not existing event when certain event does not exist.

## [0.2.3] - 2022-01-03
### Changed
- Fix the issue that UniversalEventDrawer cannot draw event dropdown in serializable list.

## [0.2.2] - 2021-12-30
### Changed
- Fixed some small bug. Update CI/CD rules.

## [0.2.1] - 2021-12-11
### Added
- Added CI/CD for the documentation build.

## [0.2.0] - 2021-10-31
### Added
- Added universal event framework, support custom event system.
- Implement the old Global Event System using the new event framework.
### Changed
- Rename the whole project from Global Event System to Universal Event System.

## [0.1.2] - 2021-10-15
### Added
- Added UnRegister method to unregister method from global event.
### Changed
- Fixed the singleton error when there's already a GlobalEventManager instance.
- Fixed the build error caused by the editor usage.


## [0.1.1] - 2021-08-21
### Changed
- Fixed the config file save failed error.

## [0.1.0] - 2021-08-20
### Added
- Finished event path view.
- Added save and load function.
- Added directory expand and fold view.

## [0.0.1] - 2021-08-19
### Added
- Initialize repository.
- Finish implement basic invoke and listen function.

[Unreleased]: https://github.com/Fangjun-Zhou/TOKMAK-Universal-Event
[0.2.6]: https://github.com/Fangjun-Zhou/TOKMAK-Universal-Event-Release/releases/tag/v0.2.6
[0.2.5]: https://github.com/Fangjun-Zhou/TOKMAK-Universal-Event-Release/releases/tag/v0.2.5
[0.2.4]: https://github.com/Fangjun-Zhou/TOKMAK-Universal-Event-Release/releases/tag/v0.2.4
[0.2.3]: https://github.com/Fangjun-Zhou/TOKMAK-Universal-Event-Release/releases/tag/v0.2.3
[0.2.2]: https://github.com/Fangjun-Zhou/TOKMAK-Universal-Event-Release/releases/tag/v0.2.2
[0.2.1]: https://github.com/Fangjun-Zhou/TOKMAK-Universal-Event-Release/releases/tag/v0.2.1
[0.2.0]: https://github.com/Fangjun-Zhou/TOKMAK-Universal-Event-Release/releases/tag/v0.2.0
[0.1.2]: https://github.com/Fangjun-Zhou/TOKMAK-Universal-Event-Release/releases/tag/v0.1.2
[0.1.1]: https://github.com/Fangjun-Zhou/TOKMAK-Universal-Event-Release/releases/tag/v0.1.1
[0.1.0]: https://github.com/Fangjun-Zhou/TOKMAK-Universal-Event-Release/releases/tag/v0.1.0
[0.0.1]: https://github.com/Fangjun-Zhou/TOKMAK-Universal-Event-Release/releases/tag/v0.0.1
