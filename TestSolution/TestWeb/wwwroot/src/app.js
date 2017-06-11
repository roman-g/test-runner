import React from 'react';
import { render } from 'react-dom'
import Dashboard from './Dashboard';

const rootElement = (<Dashboard></Dashboard>);
render(rootElement, document.getElementById('root'));