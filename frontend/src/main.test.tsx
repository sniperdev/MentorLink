import { render, screen } from '@testing-library/react';
import { MemoryRouter, Route, Routes } from 'react-router';
import App from './App';

describe('Routing', () => {
	it('renders the App component for the root route', () => {
		render(
			<MemoryRouter initialEntries={['/']}>
				<Routes>
					<Route path="/" element={<App />} />
				</Routes>
			</MemoryRouter>
		);

		expect(screen.getByText(/Click on the Vite and React logos to learn more/i)).toBeInTheDocument();
	});
});
