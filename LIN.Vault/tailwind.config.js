/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["../**/*{html,razor,js,cs}"],
    theme: {
        screens: {
            'sm': '640px',
            'md': '768px',
            'dl': '910px',
            'lg': '1024px',
            'xl': '1280px',
            '2xl': '1536px',
        },
        extend: {
            colors: {

                'current': {
                    '50': '#eff5ff',
                    '100': '#dbe8fe',
                    '200': '#bfd7fe',
                    '300': '#93bbfd',
                    '400': '#609afa',
                    '500': '#3b82f6',
                    '600': '#2570eb',
                    '700': '#1d64d8',
                    '800': '#1e55af',
                    '900': '#1e478a',
                    '950': '#172e54'
                }
            }

        }
    },
    plugins: [],
}