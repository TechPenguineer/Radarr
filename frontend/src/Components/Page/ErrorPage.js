import PropTypes from 'prop-types';
import React from 'react';
import getErrorMessage from 'Utilities/Object/getErrorMessage';
import styles from './ErrorPage.css';

function ErrorPage(props) {
  const {
    version,
    isLocalStorageSupported,
    moviesError,
    customFiltersError,
    tagsError,
    qualityProfilesError,
    languagesError,
    uiSettingsError,
    systemStatusError
  } = props;

  let errorMessage = 'Failed to load Radarr';

  if (!isLocalStorageSupported) {
    errorMessage = 'Local Storage is not supported or disabled. A plugin or private browsing may have disabled it.';
  } else if (moviesError) {
    errorMessage = getErrorMessage(moviesError, 'Failed to load movie from API');
  } else if (customFiltersError) {
    errorMessage = getErrorMessage(customFiltersError, 'Failed to load custom filters from API');
  } else if (tagsError) {
    errorMessage = getErrorMessage(tagsError, 'Failed to load tags from API');
  } else if (qualityProfilesError) {
    errorMessage = getErrorMessage(qualityProfilesError, 'Failed to load quality profiles from API');
  } else if (languagesError) {
    errorMessage = getErrorMessage(languagesError, 'Failed to load languages from API');
  } else if (uiSettingsError) {
    errorMessage = getErrorMessage(uiSettingsError, 'Failed to load UI settings from API');
  } else if (systemStatusError) {
    errorMessage = getErrorMessage(uiSettingsError, 'Failed to load system status from API');
  }

  return (
    <div className={styles.page}>
      <div className={styles.errorMessage}>
        {errorMessage}
      </div>

      <div className={styles.version}>
        Version {version}
      </div>
    </div>
  );
}

ErrorPage.propTypes = {
  version: PropTypes.string.isRequired,
  isLocalStorageSupported: PropTypes.bool.isRequired,
  moviesError: PropTypes.object,
  customFiltersError: PropTypes.object,
  tagsError: PropTypes.object,
  qualityProfilesError: PropTypes.object,
  languagesError: PropTypes.object,
  uiSettingsError: PropTypes.object,
  systemStatusError: PropTypes.object
};

export default ErrorPage;
