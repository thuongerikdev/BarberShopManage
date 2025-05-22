import 'package:barbermanagemobile/domain/repositories/promotion_repository.dart';

class CreateCustomerPromotionUseCase {
  final PromotionRepository repository;

  CreateCustomerPromotionUseCase(this.repository);

  Future<void> call(int customerId, int promoId, String status) async {
    await repository.createCustomerPromotion(customerId, promoId, status);
  }
}