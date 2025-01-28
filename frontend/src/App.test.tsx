import { render, screen, fireEvent } from '@testing-library/react';
import App from './App';

test('renders Vite + React heading and increments count on button click', () => {
	render(<App />);

	const headingElement = screen.getByText(/Vite \+ React/i);
	expect(headingElement).toBeInTheDocument();

	const buttonElement = screen.getByRole('button', { name: /count is 0/i });
	expect(buttonElement).toBeInTheDocument();

	fireEvent.click(buttonElement);
	expect(buttonElement).toHaveTextContent('count is 1');
});