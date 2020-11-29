import '@testing-library/jest-dom';
import '@testing-library/jest-dom/extend-expect';

Object.defineProperty(window, 'matchMedia', {
  value: () => {
    return {
      matches: false,
      addListener: () => {},
      removeListener: () => {}
    };
  }
});

// window.alert = jest.fn();

// jest.spyOn(global.console, 'log').mockImplementation(jest.fn());
