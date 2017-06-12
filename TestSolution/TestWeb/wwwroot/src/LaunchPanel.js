import React from 'react';
import axios from 'axios';

export default class LaunchPanel extends React.Component
{
    constructor(){
        super();
        this.state = {
            branch: "default",
            server: "http://localhost:8000",
            dll: "TestTest\\TestTest\\bin\\Debug\\TestTest.dll"
        };
    }

    runBranch(){
       axios.post("/api/Tests/Run", {Branch: this.state.branch, Server: this.state.server, Dll: this.state.dll});
    }

    onBranchInputChange(event){
        const newValue = event.target.value;
        this.setState(s => ({...s, branch: newValue}))
    }

    onServerInputChange(event){
        const newValue = event.target.value;
        this.setState(s => ({...s, server: newValue}))
    }

    onDllInputChange(event){
        const newValue = event.target.value;
        this.setState(s => ({...s, dll: newValue}))
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
            <div>
                <label htmlFor="dllInput">Server</label>
                <input id="dllInput" value={this.state.dll} onChange={e => this.onDllInputChange(e)}></input>
            </div>
            <button onClick={() => this.runBranch()}>Run</button>
        </div>);
    }
}