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
