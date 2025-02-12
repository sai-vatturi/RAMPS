import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AiFoodRecommendationComponent } from './ai-food-recommendation.component';

describe('AiFoodRecommendationComponent', () => {
	let component: AiFoodRecommendationComponent;
	let fixture: ComponentFixture<AiFoodRecommendationComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [AiFoodRecommendationComponent]
		}).compileComponents();

		fixture = TestBed.createComponent(AiFoodRecommendationComponent);
		component = fixture.componentInstance;
		fixture.detectChanges();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
