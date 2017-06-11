import React from 'react';

export default class LaunchPanel extends React.Component
{
    render(){
        return (<div>
            <label htmlFor="branchInput">Branch</label>
            <input id="branchInput"></input>
            <button>Run</button>
        </div>);
    }
}