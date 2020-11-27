import { configureStore, getDefaultMiddleware } from '@reduxjs/toolkit';
import rxPricesReducer from '../features/rxPrices/rxPricesSlice';
import usersReducer from '../features/users/usersSlice';

export default configureStore({
  reducer: {
    rxPrices: rxPricesReducer,
    users: usersReducer
  },
  middleware: getDefaultMiddleware()
});
