-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Dec 17. 17:28
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `kalaplengetőverseny_pdd`
--
CREATE DATABASE IF NOT EXISTS `kalaplengetőverseny_pdd` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_hungarian_ci;
USE `kalaplengetőverseny_pdd`;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `versenyzok`
--

CREATE TABLE IF NOT EXISTS `versenyzok` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Nev` varchar(256) NOT NULL,
  `pillanatnyihelyezes` int(11) NOT NULL,
  `pont1` int(11) NOT NULL,
  `ido1` double NOT NULL,
  `pont2` int(11) NOT NULL,
  `ido2` double NOT NULL,
  `pont3` int(11) NOT NULL,
  `ido3` double NOT NULL,
  `legjobbpont` int(11) NOT NULL,
  `legjobbido` double NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `ID` (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_hungarian_ci;

--
-- A tábla adatainak kiíratása `versenyzok`
--

INSERT INTO `versenyzok` (`ID`, `Nev`, `pillanatnyihelyezes`, `pont1`, `ido1`, `pont2`, `ido2`, `pont3`, `ido3`, `legjobbpont`, `legjobbido`) VALUES
(1, 'Kovács János', 5, 7, 3.25, 8, 2.1, 6, 4.5, 8, 2.1),
(2, 'Nagy Eszter', 3, 9, 1.85, 8, 2, 7, 3.75, 9, 1.85),
(3, 'Tóth Péter', 8, 6, 4.5, 5, 5.2, 6, 3.9, 6, 3.9),
(4, 'Szabó Anna', 4, 8, 2.5, 8, 2.2, 9, 1.95, 9, 1.95),
(5, 'Farkas Gábor', 6, 7, 3.75, 8, 2.85, 8, 2.3, 8, 2.3),
(6, 'Kis Ferenc', 11, 5, 5, 6, 4, 3, 7, 6, 4),
(7, 'Vajas László', 12, 5, 5.4, 5, 5.6, 2, 8.9, 5, 5.4),
(8, 'Mákos János', 7, 1, 9, 7, 3.6, 6, 4.7, 7, 3.6),
(9, 'Hangos Hanga', 10, 3, 7.5, 6, 4, 5, 5.8, 6, 4),
(10, 'Pocak Laci', 2, 7, 3.6, 10, 0.89, 8, 2.7, 10, 0.89),
(11, 'Lapos Károly', 1, 10, 0.67, 8, 2.7, 9, 1.3, 10, 0.67),
(12, 'Almási József', 9, 6, 4, 1, 9, 4, 6.7, 6, 4);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
