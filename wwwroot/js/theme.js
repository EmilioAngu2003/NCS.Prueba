(() => {
    'use strict'

    const getAutoTheme = () => window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'
    const getStoredTheme = () => {
        try {
            return localStorage.getItem('theme')
        } catch (e) {
            return null
        }
    }
    const setStoredTheme = (theme) => {
        try {
            localStorage.setItem('theme', theme)
        } catch (e) { }
    }

    const applyTheme = (theme) => {
        const activeTheme = (!theme || theme === 'auto') ? getAutoTheme() : theme;
        document.documentElement.setAttribute('data-bs-theme', activeTheme)
    }

    applyTheme(getStoredTheme())

    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', () => {
        const stored = getStoredTheme()
        if (!stored || stored === 'auto') applyTheme('auto')
    })

    window.addEventListener('storage', (e) => {
        if (e.key === 'theme') applyTheme(e.newValue)
    })

    window.selectTheme = (theme) => {
        setStoredTheme(theme)
        applyTheme(theme)
    }
})()