-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Máy chủ: 127.0.0.1
-- Thời gian đã tạo: Th12 09, 2024 lúc 10:51 AM
-- Phiên bản máy phục vụ: 10.4.32-MariaDB
-- Phiên bản PHP: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Cơ sở dữ liệu: `comp1551_quizgame`
--

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `leaderboard`
--

CREATE TABLE `leaderboard` (
  `Id` int(11) NOT NULL,
  `PlayerName` varchar(100) NOT NULL,
  `Score` int(11) NOT NULL,
  `TotalQuestions` int(11) NOT NULL,
  `TimeSpent` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `leaderboard`
--

INSERT INTO `leaderboard` (`Id`, `PlayerName`, `Score`, `TotalQuestions`, `TimeSpent`) VALUES
(12, 'Nguyen', 11, 15, 117.51944789999999),
(13, 'gggg', 2, 15, 19.1135664),
(14, 'Vu', 7, 15, 173.8342615),
(15, 'vu', 1, 1, 3.2044281999999997);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `questions`
--

CREATE TABLE `questions` (
  `Id` int(11) NOT NULL,
  `QuestionType` varchar(20) NOT NULL,
  `QuestionText` text NOT NULL,
  `Options` text DEFAULT NULL,
  `CorrectAnswer` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `questions`
--

INSERT INTO `questions` (`Id`, `QuestionType`, `QuestionText`, `Options`, `CorrectAnswer`) VALUES
(14, 'OpenEnded', 'What is the capital of France?', NULL, 'paris'),
(17, 'OpenEnded', 'Which continent is the Sahara Desert located in?', NULL, 'africa'),
(18, 'OpenEnded', 'What is the longest river in the world?', NULL, 'nile'),
(19, 'OpenEnded', 'Which ocean is the largest?', NULL, 'pacific ocean'),
(20, 'OpenEnded', 'What country has the most islands?', NULL, 'sweden'),
(21, 'MultipleChoice', 'Which is the smallest country in the world by land area?', 'Monaco|Vatican City|San Marino|Liechtenstein', '1'),
(22, 'MultipleChoice', 'Which continent has the most countries?', 'Asia|Africa|Europe|South America', '1'),
(23, 'MultipleChoice', 'What is the largest desert in the world?', 'Gobi Desert|Arabian Desert|Sahara Desert|Antarctic Desert', '3'),
(24, 'MultipleChoice', 'Which city is known as the \'City of Eternal Spring\'?', 'Hanoi|Da Nang|Da Lat|Hue', '2'),
(25, 'MultipleChoice', 'What is the most famous tunnel system used during the Vietnam War?', 'Cu Chi Tunnels|Vinh Moc Tunnels|Ho Chi Minh Tunnels|Phong Nha Tunnels', '0'),
(26, 'TrueFalse', 'Hoang Sa, Truong Sa is of Viet Nam', NULL, '1'),
(27, 'TrueFalse', 'Vietnam has a tropical monsoon climate?', NULL, '1'),
(28, 'TrueFalse', 'Ha Long Bay is located in southern Vietnam?', NULL, '0'),
(29, 'TrueFalse', 'The Mekong River flows through the capital of Vietnam?', NULL, '0'),
(30, 'TrueFalse', 'Vietnam is known for its coffee production?', NULL, '1');

--
-- Chỉ mục cho các bảng đã đổ
--

--
-- Chỉ mục cho bảng `leaderboard`
--
ALTER TABLE `leaderboard`
  ADD PRIMARY KEY (`Id`);

--
-- Chỉ mục cho bảng `questions`
--
ALTER TABLE `questions`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT cho các bảng đã đổ
--

--
-- AUTO_INCREMENT cho bảng `leaderboard`
--
ALTER TABLE `leaderboard`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT cho bảng `questions`
--
ALTER TABLE `questions`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
