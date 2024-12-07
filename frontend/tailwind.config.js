/** @type {import('tailwindcss').Config} */
module.exports = {
	content: [
	  "./src/**/*.{html,ts}",
	],
	theme: {
	  extend: {
		fontFamily: {
			gilroy: ['Gilroy', 'sans-serif'],  // Add Gilroy font to Tailwind theme
		  },
		  fontWeight: {
			thin: 100,
			ultraLight: 200,
			light: 300,
			regular: 400,
			medium: 500,
			semiBold: 600,
			bold: 600,
			extraBold: 800,
			heavy: 900,
		  },
		colors: {
		  primary: '#4f46e5', // Example primary color
		  secondary: '#9333ea',
		  'brand-green': '#80C522',
		  'dark-green': '#5B6056',
		}
	  }
	},
	plugins: [
		require('tailwind-scrollbar'),
	],
}
