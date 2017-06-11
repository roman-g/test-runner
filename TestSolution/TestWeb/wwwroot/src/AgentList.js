import React from 'react';
import axios from 'axios';

export default class AgentList extends React.Component
{
    constructor() {
        super();
        this.state = {agents: []};
    }

    componentDidMount(){
        axios.get("/api/AgentList")
            .then(response => {
                this.setState(s => ({...s, agents: response.data.names}));
            });
    }

    render(){
        const agents = this.state.agents.map(x => <div key={x}>{x}</div>);

        return (<div>
            {agents}
        </div>);
    }
}