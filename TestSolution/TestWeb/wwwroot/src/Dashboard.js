import React from 'react';
import LaunchPanel from './LaunchPanel';
import AgentList from './AgentList';

export default class Dashboard extends React.Component
{
    render(){
        return (
            <div>
                <LaunchPanel></LaunchPanel>
                <AgentList></AgentList>
            </div>)
    }
}