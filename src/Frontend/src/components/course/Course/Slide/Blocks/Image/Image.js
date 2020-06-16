import React from "react";

import ImageGallery from 'react-image-gallery';

import classNames from "classnames";
import PropTypes from 'prop-types';

import styles from './Image.less';

import 'react-image-gallery/styles/css/image-gallery.css';

class Image extends React.Component {
	constructor(props) {
		super(props);

		this.state = {
			fullscreen: false,
			showFullscreenButton: true,
			width: null,
		}
	}

	render() {
		const { imageUrls, className, } = this.props;
		const { fullscreen, showFullscreenButton, width, } = this.state;

		const wrapperStyle = width
			? { width: width + 'px' }
			: { opacity: 0, height: 0, }; //prevent showing extended img

		return (
			<div className={ classNames(styles.wrapper, className) } style={ wrapperStyle }>
				<ImageGallery
					onImageLoad={ this.onImageLoad }
					additionalClass={ classNames(styles.imageWrapper, { [styles.open]: fullscreen }) }
					useBrowserFullscreen={ false }
					showBullets={ imageUrls.length !== 1 }
					showFullscreenButton={ showFullscreenButton }
					showPlayButton={ false }
					showThumbnails={ false }
					onScreenChange={ this.onScreenChange }
					items={ imageUrls.map(url => {
						return { original: url, };
					}) }/>
			</div>
		);
	}

	onImageLoad = (event) => {
		const img = event.target;

		if(this.state.width === null && img.width > img.naturalWidth) //set all img width to real size of first img, if not bigger than slide width
			this.setState({
				width: img.naturalWidth,
				showFullscreenButton: false,
			})
	}

	onScreenChange = (isFullScreen) => {
		this.setState({
			fullscreen: isFullScreen,
		})
	}
}

Image.propTypes = {
	className: PropTypes.string,
	imageUrls: PropTypes.arrayOf(PropTypes.string),
}

export default Image;