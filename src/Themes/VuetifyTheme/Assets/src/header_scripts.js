import outdatedBrowser from "outdated-browser-rework";
import "outdated-browser-rework/dist/style.css";
import IsDarkMode from './plugins/darkMode';

// set the body's background to be dark if DarkMode is currently enabled. To avoid a flash.
if(IsDarkMode()) {
  document.documentElement.style.backgroundColor = "#121212";
}

outdatedBrowser({
	browserSupport: {
		Chrome: 57,
		Edge: 39,
		Safari: 10,
		"Mobile Safari": 10,
		Firefox: 50,
		Opera: 50,
		Vivaldi: 1,
		IE: false
	},
	requireChromeOnAndroid: false,
	isUnknownBrowserOK: true,
  language: document.documentElement.lang,
	messages: {
		en: {
			outOfDate: "Your browser is no longer supported!",
			update: {
				web: `Please use a modern browser view this website correctly.
          Supported browsers include:
          <ul>
            <li>Chrome & Chromium</li>
            <li>Firefox</li>
            <li>Edge</li>
            <li>Safari</li>
          </ul>
        
        `,
				googlePlay: "Please install Chrome from Google Play",
				appStore: "Please update iOS from the Settings App"
			},
			url: null
		},
    fr: {
			outOfDate: "Votre navigateur n'est pas supporté!",
			update: {
				web: `Veuillez utiliser un navigateur moderne pour visualiser correctement ce site web.
        Les navigateurs suivants sont supportés:
        <ul>
          <li>Chrome et Chromium</li>
          <li>Firefox</li>
          <li>Edge</li>
          <li>Safari</li>
        </ul>
        `,
				googlePlay: "Merci d'installer Chrome depuis le Google Play Store",
				appStore: "Merci de mettre à jour iOS depuis l'application Réglages"
			},
			url: null
		}
	}
});
 