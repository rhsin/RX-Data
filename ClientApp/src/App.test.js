import React from 'react';
import { render, screen } from '@testing-library/react';
import { Provider } from 'react-redux';
import axios from 'axios';
import store from './app/store';
import App from './App';

jest.mock('axios');

test('renders app & fetches data from API', () => {
  render(
    <Provider store={store}>
      <App />
    </Provider>
  );

  expect(screen.getByText(/RxData/i)).toBeInTheDocument();
  expect(axios.get).toHaveBeenCalledTimes(2);
});
