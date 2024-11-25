import DetectorBase from "./detectorBase";

const SEARCH_PATTERN = "**/*.epicchain-checkpoint";

export default class CheckpointDetector extends DetectorBase {
  get checkpointFiles() {
    return this.files;
  }

  constructor() {
    super(SEARCH_PATTERN);
  }
}
