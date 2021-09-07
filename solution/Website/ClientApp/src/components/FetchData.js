import React, { Component } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true };
  }

  componentDidMount() {
    this.populateWeatherData();
    this.createHubConnection();
  }

  componentDidUpdate(prevProps, prevState) {
    console.log('componentDidUpdate', prevProps, prevState);
    console.log('hubConnection in componentDidUpdate', prevState.hubConnection, this.state.hubConnection);
}

  createHubConnection() {
    const hubConnection = new HubConnectionBuilder()
          .withUrl('http://localhost:4000/hub')
          .withAutomaticReconnect()
          .build();
    
    console.log('hubConnection', hubConnection);

    hubConnection.start().then(result => {
      console.log('Connected!');

      hubConnection.on('DisplayNotification', (user, message) => {
        const json = JSON.parse(message);
        console.log('DisplayNotification', user, message, json);
        toast(() => <img src={json.weatherIconUrl} alt="" />);
      });
    });

    //this.setState(prevState => ({ ...prevState, hubConnection }));
  }

  static renderForecastsTable(forecasts) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp. (C)</th>
            <th>Temp. (F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          {forecasts.map(forecast =>
            <tr key={forecast.date}>
              <td>{forecast.date}</td>
              <td>{forecast.temperatureC}</td>
              <td>{forecast.temperatureF}</td>
              <td>{forecast.summary}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  loadMore = () => this.populateWeatherData();

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderForecastsTable(this.state.forecasts);

    return (
      <div>
        <h1 id="tabelLabel" >Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
        <div>
          <button onClick={this.loadMore}>Load More</button>
        </div>
        {contents}
        <ToastContainer />
      </div>
    );
  }

  async populateWeatherData() {
    const response = await fetch('weatherforecast');
    const data = await response.json();
    console.log('data', data, this.state.forecasts, [...this.state.forecasts, ...data]);
    this.setState(prevState => ({ ...prevState, forecasts: [...prevState.forecasts, ...data], loading: false }));
  }
}
