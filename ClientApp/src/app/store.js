import { configureStore, getDefaultMiddleware } from '@reduxjs/toolkit';
import counterReducer from '../features/counter/counterSlice';
import rxPricesReducer from '../features/rxPrices/rxPricesSlice';

export default configureStore({
  reducer: {
    counter: counterReducer,
    rxPrices: rxPricesReducer
  },
  middleware: getDefaultMiddleware()
});
