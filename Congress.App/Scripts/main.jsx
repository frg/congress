var Post = React.createClass({
	render: function() {
		return (
			<div className="row">
				<div className="small-2 medium-1 columns vote-container">
					<i className="fa fa-arrow-up" data-vote="1"></i>
					<div>23</div>
					<i className="fa fa-arrow-down" data-vote="-1"></i>
				</div>
				<div className="small-10 medium-11 columns">
					<div className="row">
						<div className="small-12 columns">
							<h5>Sample Title</h5>
						</div>
					</div>
					<div className="row">
						<div className="small-12 columns">
							<h5><small>Yesterday</small></h5>
						</div>
					</div>
				</div>
			</div>
		);
	}
});

React.render(
  <Post />,
  document.getElementById('posts-container')
);