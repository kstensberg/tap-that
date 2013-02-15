--
-- Database: `8bitideas`
--

-- --------------------------------------------------------

--
-- Table structure for table `tapthat-deltas`
--

CREATE TABLE IF NOT EXISTS `tapthat-deltas` (
  `id` int(7) NOT NULL AUTO_INCREMENT,
  `userId` int(7) NOT NULL,
  `delta` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `userId` (`userId`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE IF NOT EXISTS `users` (
  `id` int(7) NOT NULL AUTO_INCREMENT,
  `name` varchar(256) NOT NULL,
  `username` varchar(128) NOT NULL,
  `password` varchar(256) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1;
