import React from 'react';
import { render, screen, waitForElementToBeRemoved } from '@testing-library/react';
import { Provider } from 'react-redux';
import axios from 'axios';
import store from './app/store';
import App from './App';

jest.mock('axios');

test('renders app & fetches data from API', async () => {
  render(
    <Provider store={store}>
      <App />
    </Provider>
  );
  
  await waitForElementToBeRemoved(()=> screen.getAllByText(/Loading/i));
  
  expect(screen.getByText(/RxData/i)).toBeInTheDocument();
  expect(axios.get).toHaveBeenCalledTimes(2);
});

test('renders user table headings', async () => {
  render(
    <Provider store={store}>
      <App />
    </Provider>
  );
  
  expect(screen.getByText(/Email/i)).toBeInTheDocument();
});

test('renders rxPrice table headings', async () => {
  render(
    <Provider store={store}>
      <App />
    </Provider>
  );
  
  expect(screen.getAllByText(/Vendor/i)).toHaveLength(2);
});

test('renders medication search input', async () => {
  render(
    <Provider store={store}>
      <App />
    </Provider>
  );
  
  expect(screen.getByPlaceholderText(/Enter Medication/i)).toBeInTheDocument();
});

test('renders medication find button', async () => {
  render(
    <Provider store={store}>
      <App />
    </Provider>
  );
  
  expect(screen.getByRole('button', {name: /Find/i})).toBeInTheDocument();
});