import React from 'react';
import LaunchPanel from './LaunchPanel';
import AgentsList from './AgentsList';

export default class Dashboard extends React.Component
{
    render(){
        return (
            <div>
                <LaunchPanel></LaunchPanel>
                <AgentsList></AgentsList>
            </div>)
    }
}