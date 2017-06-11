import React from 'react';
import axios from 'axios';

export default class LaunchPanel extends React.Component
{
    constructor(){
        super();
        this.state = {
            branch: "",
            server: ""
        };
    }

    runBranch(){
       axios.post("/api/Tests/Run", {Branch: this.state.branch, Server: this.state.server});
    }

    onBranchInputChange(event){
        const newValue = event.target.value;
        this.setState(s => ({...s, branch: newValue}))
    }

    onServerInputChange(event){
        const newValue = event.target.value;
        this.setState(s => ({...s, server: newValue}))
    }

    render(){
        return (<div>
            <div>
                <label htmlFor="branchInput">Branch</label>
                <input id="branchInput" value={this.state.branch} onChange={e => this.onBranchInputChange(e)}></input>
            </div>
            <div>
                <label htmlFor="serverInput">Server</label>
                <input id="serverInput" value={this.state.server} onChange={e => this.onServerInputChange(e)}></input>
            </div>
            <button onClick={() => this.runBranch()}>Run</button>
        </div>);
    }
}