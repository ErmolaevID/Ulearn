import { RateTypes } from "src/consts/rateTypes";

interface Flashcard {
	id: string,
	question: string,
	answer: string,
	unitTitle: string,
	rate: RateTypes,
	unitId: string,
	theorySlidesIds: string[],
	lastRateIndex: number,
	theorySlides: TheorySlideInfo[],
}

interface UnitFlashcardsInfo {
	unitId: string,
	unitTitle: string,
	unlocked: boolean,
	flashcardsIds: string[],
	unratedFlashcardsCount: number,
	cardsCount: number,
	flashcardsSlideSlug: string,
}

interface TheorySlideInfo {
	slug: string,
	title: string,
}

interface QuestionWithAnswer {
	question: string,
	answer: string,
}

export { Flashcard, UnitFlashcardsInfo, TheorySlideInfo, QuestionWithAnswer, };
