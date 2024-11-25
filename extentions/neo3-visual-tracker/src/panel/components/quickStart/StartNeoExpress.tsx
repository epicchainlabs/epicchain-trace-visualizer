import React from "react";

import NavButton from "../NavButton";

type Props = {
  onStart: () => void;
};

export default function startEpicChainExpress({ onStart }: Props) {
  return (
    <>
      <div style={{ margin: 10, textAlign: "left" }}>
        You are not currently running an instance of EpicChain Express. Running Neo
        Express will allow you to deploy, test and debug your contracts locally.
      </div>
      <NavButton style={{ margin: 10 }} onClick={onStart}>
        Start EpicChain Express
      </NavButton>
    </>
  );
}
